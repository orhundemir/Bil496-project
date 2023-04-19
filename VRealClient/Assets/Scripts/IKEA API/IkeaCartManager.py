import ikea_api
from selenium import webdriver
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC
from selenium import webdriver
from webdriver_manager.chrome import ChromeDriverManager
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.chrome.service import Service
import time

def add_items(objs):
    constants = ikea_api.Constants(country="us", language="en")
    session_info = ikea_api.Auth(constants)
    token = ikea_api.run(session_info.get_guest_token())
    cart = ikea_api.Cart(constants, token=str(token))
    for obj in objs:
        ikea_api.run(cart.add_items({obj: 1}))  # { item_code: quantity }
        #ikea_api.run(cart.add_items({"80214549": 1}))  # { item_code: quantity }
    cart_show = ikea_api.run(cart.show())
    items = ikea_api.convert_cart_to_checkout_items(cart_show)
   
    time.sleep(2)
   
    chrome_options = Options()
    chrome_options.add_experimental_option("detach", True)
    browser = webdriver.Chrome(service=Service(ChromeDriverManager().install()), chrome_options=chrome_options)
    
    browser.implicitly_wait(10)
    browser.get(constants.local_base_url + "/shoppingcart")
    WebDriverWait(browser, 10).until(EC.element_to_be_clickable((By.XPATH,'//*[@id="onetrust-accept-btn-handler"]'))).click()
    browser.delete_cookie("guest")
    browser.add_cookie({
        'name' : 'guest',
        'value' : token
    })
    browser.refresh()

add_items(["80214549"])