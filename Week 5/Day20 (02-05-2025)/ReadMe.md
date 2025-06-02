# JwtToken
 - Here, the AppointmentApi previously created was modified in their db structure to implement the user authentication feature
 - The token can be created by the users of the app by providing the email as username and password
 - The AutoMapper is used to map the Models with the User table
 - The hMACSHA256 algo is used to encrypt the password and to create the token

# Authentication
 - The routes are protected to allow access to only authorised users
 - Now in this app, those who are doctors and patients can be authenticate to get the token and access the apis
