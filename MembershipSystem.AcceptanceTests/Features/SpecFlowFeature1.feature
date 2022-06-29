Feature: SpecFlowFeature1

Scenario: Logging in
	Given the employees card is registered
	When the user logs in 
	Then they should receive a ok result

Scenario: Logging in while not registered
	Given the employees card is not registered
	When the user logs in while not being registered 
	Then they should receive a bad request result

Scenario: Logging in with incorrect card
	Given the employees card is registered
	When the emlopyee enters an incorrect card id
	Then they should be told the card does not exist

Scenario: Enters incorrect pin
	Given the employees card is registered
	When they enter an incorrect pin
	Then They should receive a authentication error

Scenario: Registering
	Given the employees card is not registered
	When the user registers with valid inputs
	Then they should receive a ok result

Scenario: Registering with an incorrect email
	Given the employees card is not registered
	When the user registers with an invalid email
	Then they should be told 'Email is invalid'

Scenario: Registering with an incorrect numver
	Given the employees card is not registered
	When the user registers with an invalid phone number
	Then they should be told 'Invalid Phone Number'

Scenario: Registering with a null value
	Given the employees card is not registered
	When the user registers with a null value
	Then they should be told 'You have inputted a empty value'

Scenario: Adding money
	Given the employees card is registered
	And the user is logged in
	When they attempted to add £5
	Then Their balance should increase by £5
	And they should receive a ok result

Scenario: Adding money after the timeout period
	Given the employees card is registered and the user is logged in and they have not done anything on the system for 10 mins
	When they attempted to add £5
	Then They should receive a timeout error

Scenario: Buying something
	Given the employees card is registered And they have £10 in their account
	And the user is logged in
	When they attempt to buy something for £2
	Then Their balance should decrease by £2
	And they should receive a ok result


Scenario: Trying to buy something you cant afford
	Given the employees card is registered And they have £1 in their account
	And the user is logged in
	When they attempt to buy something for £5
	Then they should receive a bad request result


Scenario: Buy something after the timeout period
	Given the employees card is registered and the user is logged in and they have not done anything on the system for 13 mins
	When they attempt to buy something for £5
	Then They should receive a timeout error

Scenario: Logging off
	Given the employees card is registered
	And the user is logged in
	When the user logs off
	Then they should receive a ok result

Scenario: Logging off while not logged in
	Given the employees card is registered
	And the user is not logged in
	When the user logs off while having not logged in
	Then they should receive a bad request result