For this project,
Some assumptions were made:

1. Only products with Normal category will consider "DaysInAdvance" property for days calculation.
2. For External category, even if "DaysInAdvance" property is present, it will not be considered for calculation of delivery dates and default value 5 will be considered for calculation as mentioned in the test.
3. For Temporary category, the remaining number of days of the week are considered for delivery date calculation and "DaysInAdvance" field value is ignored. Eg. If user is ordering on Wednesday, then thursday to sunday , i.e. 4 days are considered as number of days before we can deliver the items.
4. Assuming that user can not add temporary product in the cart, if one is ordering the things on Sunday. So no validation for this is added in the code.
5. A simple JWT token implementation is done in api project, which was not asked:). To generate the token following credentials can be used.
    1. Username: "MathemAdmin" 

       Password: "Mathem@9025"

    2. Username: "MathemUser1"

       Password: "Mathem@123"


## How to run the project / api 

Project is divided into 3 parts. 
1. DeliveryDatesGenerator : The core logic for Delivery dates calculation
2. MathemWebApi : Simple apis, for generating token and to return the delivery dates
3. DeliveryDatesGeneratorTests: Test project to cover the test cases

Set "MathemWebApi" project as start-up project and run. This will open up the swagger file.

In the "​/api​/Account​/GetToken" method, provide username and password combination specified above, and generate the token.

Then use this generated token, to authorize in swagger page.

Once authorizaiton is done, provide the required inputs to "​/api​/GetDeliveryDates" endpoint, to get the result.

## Request Inputs
### __Postalcode__: 
It expects a string.
I have added basic regx validation for swedens postal code. This I copied from net.

__valid values__: 12345 ||| 932 68 ||| S-621 46

__Invalid values__: 5367 ||| 425611 ||| 31 545

### __Payload/Request Body__:
This expects a list of products. 
```
[
  {
    "productId": 0,
    "name": "string",
    "deliveryDays": [
      0
    ],
    "typeOfProduct": 0,
    "daysInAdvance": 0,
    "quantity": 0
  }
]
```

- productId: Int
- name: string
- deliveryDays: [array of DaysOfWeek]. eg [ "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]
- typeOfProduct: ProductType enum. eg. "Normal" / "External" / "Temporary" 
- quantity: int

## Payload for various scenarios:
- When all products are normal products:
 ```
[
  {
    "productId": 1,
    "name": "Banana",
    "deliveryDays": [
      "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    "typeOfProduct": "Normal",
    "daysInAdvance": 2,
    "quantity": 6
  },
    {
    "productId": 2,
    "name": "Mango",
    "deliveryDays": [
      "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    "typeOfProduct": "Normal",
    "daysInAdvance": 1,
    "quantity": 4
  }
]
```
- When all delivery days of all products are different:
 ```
[
  {
    "productId": 1,
    "name": "Banana",
    "deliveryDays": [
      "Sunday","Tuesday"
    ],
    "typeOfProduct": "Normal",
    "daysInAdvance": 2,
    "quantity": 6
  },
    {
    "productId": 2,
    "name": "Mango",
    "deliveryDays": [
      "Thursday","Friday"
    ],
    "typeOfProduct": "External",
    "daysInAdvance": 4,
    "quantity": 4
  }
]
```
- When all order contains normal and external products:
 ```
[
  {
    "productId": 1,
    "name": "Banana",
    "deliveryDays": [
      "Sunday","Tuesday"
    ],
    "typeOfProduct": "Normal",
    "daysInAdvance": 2,
    "quantity": 6
  },
    {
    "productId": 2,
    "name": "Mango",
    "deliveryDays": [
      "Wednesday","Tuesday"
    ],
    "typeOfProduct": "External",
    "daysInAdvance": 4,
    "quantity": 4
  }
]
```

- When all order contains normal ,external and temporary products:
 ```
[
  {
    "productId": 1,
    "name": "Banana",
    "deliveryDays": [
      "Sunday","Tuesday"
    ],
    "typeOfProduct": "Normal",
    "daysInAdvance": 2,
    "quantity": 6
  },
    {
    "productId": 2,
    "name": "Mango",
    "deliveryDays": [
      "Sunday","Thursday"
    ],
    "typeOfProduct": "External",
    "daysInAdvance": 4,
    "quantity": 4
  },
    {
    "productId": 3,
    "name": "Cherry",
    "deliveryDays": [
      "Sunday","Friday"
    ],
    "typeOfProduct": "Temporary",
    "daysInAdvance": 4,
    "quantity": 4
  }
]
```
- When duplicates products are mentioned in the product list. Duplicate product is checked by product ID.:
 ```
[
  {
    "productId": 1,
    "name": "Banana",
    "deliveryDays": [
      "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    "typeOfProduct": "Normal",
    "daysInAdvance": 2,
    "quantity": 6
  },
    {
    "productId": 1,
    "name": "Mango",
    "deliveryDays": [
      "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    "typeOfProduct": "Normal",
    "daysInAdvance": 1,
    "quantity": 4
  }
]
```







