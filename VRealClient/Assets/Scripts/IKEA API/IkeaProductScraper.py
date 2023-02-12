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
    priceDict = product['main']['items'][0]['product']['salesPrice']['current']

    # Extract the desired information from the product dictionary
    productPrice = str(priceDict['prefix']) + str(priceDict['wholeNumber']) + str(priceDict['separator']) + str(priceDict['decimals'])
    productType = product['filters'][0]['categories'][0]['name']
    productName = product['main']['items'][0]['product']['name']
    productImageURL = product['filters'][0]['categories'][0]['imageUrl']
    productMeasurements = product['main']['items'][0]['product']['itemMeasureReferenceText']

    return {"productName": productName, "productType": productType, "productPrice": productPrice, "productImageURL": productImageURL, "productMeasurements": productMeasurements}

# Retrieve data for each product, specified as a command line argument
products = []
for i in range(1, len(sys.argv)):
    products.append(fetchProductInfo(sys.argv[i]))

# Serialize the products list into a JSON string
productsJson = json.dumps(products)

# The output of this print function will be redirected to Unity
print(productsJson)
