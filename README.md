# CarniDigiSign_App
This repo is part of a three piece solution. 

Pieces are:
* [Server](https://github.com/graboskyc/CarniDigiSign_IOT) - A Docker container that contains a NodeJS REST API and storage of data in a Mongo Database
* [Editing App](https://github.com/graboskyc/CarniDigiSign_App) - You could use the REST API directly, but this utility simplifies the access to the above REST API with a UWP API *This Repo*
* [Viewing App](https://github.com/graboskyc/CarniDigiSign_IOT) - UWP app that can be deployed on a Raspberry Pi or Intel Compute Stick running Windows 10 IOT to display the digial signage stored by the server. 

The end result is a simple to deploy and manage utility for digital signage. 

![](SS/ss01.jpg)

Supported options for the type of screen are:
* Hidden - _hide_ - The record is ignored but kept in the database
* Image - _image_ - A URL to an image
* Text - _text_ - The Text field is displayed in line
* Tweet - _tweet_ - The URL to a specific tweet. The Twitter API is called to retrieve and it is formatted
* Video - _video_ - A URL to a video file supported by the UWP video element
* Web - _web_ - A URL to a website
