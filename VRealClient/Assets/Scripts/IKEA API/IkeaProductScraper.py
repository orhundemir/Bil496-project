import ikea_api as ikea
import json
import sys

def fetchProductInfo(productURLName):
    """Retrieve product information from IKEA API based on the item name"""
    # Run the API call and store the search results
    constants = ikea.Constants(country = "us", language = "en")
    search = ikea.Search(constants)
    endpoint = search.search(productURLName)
    searchResult = ikea.run(endpoint)
    
    # Extract the product dictionary from the result
    # Also check to see if the given product name is valid via a IndexOutOfBounds exception
    # This exception is raised at the end of this script because of output redirection to Unity
    try:
        product = searchResult["searchResultPage"]["products"]["main"]["items"][0]["product"]
    except IndexError:
        return None

    # Extract the desired information from the product dictionary
    productName = product["name"]
    if productName.lower() not in productURLName.lower():
        return None

    productType = product["typeName"]
    productPrice = (
        str(product["salesPrice"]["current"]["prefix"])
        + str(product["salesPrice"]["current"]["wholeNumber"])
        + str(product["salesPrice"]["current"]["separator"])
        + str(product["salesPrice"]["current"]["decimals"])
    )
    productImageURL = product["mainImageUrl"]
    productMeasurements = product["itemMeasureReferenceText"]

    return {
        "productName": productName,
        "productType": productType,
        "productPrice": productPrice,
        "productImageURL": productImageURL,
        "productMeasurements": productMeasurements,
    }

class ProductNotFoundException(Exception):
    def __init__(self, failed_products):
        message = "The following products could not be found:\n{}".format("\n".join(failed_products))
        super().__init__(message)

# Retrieve data for each product, specified as a command line argument
notFoundProducts = []
products = []
for i in range(1, len(sys.argv)):
    productInfo = fetchProductInfo(sys.argv[i])
    if productInfo != None:
        products.append(productInfo)
    else:
        notFoundProducts.append(productInfo)

# Serialize the products list into a JSON string
productsJson = json.dumps(products)

# The output of this print function will be redirected to Unity
print(productsJson)

# Send exception message for unfetched items back to Unity
# This error has to be raised at the very end to avoid prematurely terminating and skipping remaining items
if len(notFoundProducts) != 0:
    raise ProductNotFoundException(notFoundProducts)
