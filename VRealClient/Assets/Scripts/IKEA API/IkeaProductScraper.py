import ikea_api as ikea
import json
import sys

def fetchProductInfo(productName):
    """Retrieve product information from IKEA API based on the item name"""
    # Run the API call and store the result
    constants = ikea.Constants(country = "us", language = "en")
    search = ikea.Search(constants)
    endpoint = search.search(productName)
    searchResult = ikea.run(endpoint)

    # Extract the product information from the result
    product = searchResult['searchResultPage']['products']
    
    # Check to see if the item was successfully fetched from the API
    try:
        priceDict = product['main']['items'][0]['product']['salesPrice']['current']
    except IndexError:
        notFoundProducts.append(productName)
        return None
    
    # Find the index of the categories object in filters
    # This index can vary between different products
    categoryIndex = -1
    for i, filter_item in enumerate(product['filters']):
        if filter_item['id'] == 'CATEGORIES':
            categoryIndex = i

    # Extract the desired information from the product dictionary
    productPrice = str(priceDict['prefix']) + str(priceDict['wholeNumber']) + str(priceDict['separator']) + str(priceDict['decimals'])
    productType = product['filters'][categoryIndex]['categories'][0]['name']
    productName = product['main']['items'][0]['product']['name']
    productImageURL = product['filters'][categoryIndex]['categories'][0]['imageUrl']
    productMeasurements = product['main']['items'][0]['product']['itemMeasureReferenceText']

    return {"productName": productName, "productType": productType, "productPrice": productPrice, "productImageURL": productImageURL, "productMeasurements": productMeasurements}

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

# Serialize the products list into a JSON string
productsJson = json.dumps(products)

# The output of this print function will be redirected to Unity
print(productsJson)

# Send the exception message about the items that could not be fetched back to Unity
if len(notFoundProducts) != 0:
    raise ProductNotFoundException(notFoundProducts)
