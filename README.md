<!-- PROJECT SHIELDS -->
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]


<!-- PROJECT LOGO -->
<br />
<p align="center">
  <h3 align="center">RestfulQR</h3>

  <p align="center">
    QR Codes delivered by REST
    <br />
    <br />
    <a href="https://github.com/coffeexcode/RestfulQr/issues">Report Bug</a>
    ·
    <a href="https://github.com/coffeexcode/RestfulQr/issues">Request Feature</a>
  </p>
</p>



<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary><h2 style="display: inline-block">Table of Contents</h2></summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>




<!-- ABOUT THE PROJECT -->
## About The Project
RestfulQR is a simple REST API to create and serve QR codes as part of a microservices architecture. The project uses
the [QRCoder](https://github.com/codebude/QRCoder) library to create QR codes. The project aims to serve all functionality
available in QRCoder via REST calls. 


### Built With

* [QRCoder](https://github.com/codebude/QRCoder)
* [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)
* [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0)
* [Postgres](https://www.postgresql.org/)
* [Seq](https://datalust.co/seq)
* [Docker (docker-compose)](https://docs.docker.com/compose/)



<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

You will need Docker installed in order to run the project via docker-compose.
* [Docker](https://docs.docker.com/get-docker/) installed in order to run the project via `docker-compose`.
* Atleast [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0) installed to debug and run via `dotnet`
* _Optional: Visual Studo 2019_

### AWS
RestfulQr uses S3 to persist images. You will need to create a S3 bucket to use. Once you create the bucket, you need to configure the
appSettings.json file with your values.

For example,
```json
  "AWS": {
    "Profile": "your-profile",
    "Region": "your-region",
    "ProfilesLocation": "your-aws-credentials"
  },
  "S3": {
    "BucketName": "your-bucket-name"
  }
```

You will also need to set bind your docker containers with the right volume for your credentials in `docker-compose.override.yml`
### Installation & Starting

1. Clone the repo
   ```sh
   git clone https://github.com/coffeexcode/RestfulQr.git
   ```
2. Start via `docker-compose`
   ```sh
   docker-compose up
   ```

<!-- USAGE EXAMPLES -->
## Usage

This project is meant to be incredibly simple to use if you are familiar with REST. For testing and development, the
recommended tool is [Postman](https://www.postman.com/). 

_Note: These steps assume that you are running locally with the default URL settings. This implies the application is running at_ http://localhost:14000


You can generate a retrieve your first QR code using the following steps:

1. Ensure the project is running via `docker-compose`
    ```sh
    docker-compose up
    ```
2. Get an API key.

     _Note: If you are running in development, you can skip this step and replace the api key with 'development' for future steps._

    ```sh
    curl --location --request POST 'https://localhost:14000/api/v1/apikey'
    ```
3. Use the API key to generate a simple QR code from JSON data with default settings.
    ```sh
    curl --location --request POST 'https://localhost:14000/api/v1/qrcode/json' \
    --header 'X-Api-Key: YOUR-API-KEY-GOES-HERE' \
    --header 'Content-Type: application/json' \
    --data-raw '{
    "demo": "test"
    }'
    ```

    If the request was successfully, you should get a 201 CREATED response with a body like the following:
    ```json
    {
        "succeeded": true,
        "errors": null,
        "createdQrCode": {
            "filename": "3bc3d64e-05b8-4af5-a106-dcaff7815bd3.png",
            "created": "2020-12-03T12:53:09.1174748+00:00",
            "createdBy": "YOUR-API-KEY-GOES-HERE"
        },
        "data": {
            "demo": "test"
        }
    }
    ```
4. Retrieve your QR code! Pass the filename from step #3 into the URL of the following
    ```sh
    curl --location --request GET 'https://localhost:14000/images/3bc3d64e-05b8-4af5-a106-dcaff7815bd3.png' \
    --header 'X-Api-Key: development'
    ```

## Help
RestfulQR has swagger built in. While running in development, you can browse to http://localhost:14000/swagger/index.html to view 
the swagger definintion and examples of using all of the endpoints available.

<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/coffeexcode/RestfulQr/issues) for a list of proposed features (and known issues).



<!-- CONTRIBUTING -->
## Contributing

Contributions are always welcome. Contributions should be in-line with the project goals: i.e. single use scenarios or specific businesses casesly will likely be denied. Any contributions you make are **greatly appreciated**. In general, you can contribute by doing the following:

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.



<!-- CONTACT -->
## Contact

Nathan Brown - [@kilosandcoffee](https://twitter.com/kilosandcoffee)

Project Link: [https://github.com/coffeexcode/RestfulQr](https://github.com/coffeexcode/RestfulQr)



<!-- ACKNOWLEDGEMENTS -->
## Acknowledgements

* [QRCoder](https://github.com/codebude/QRCoder). This project would not have been successfully without this awesome library.




<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[issues-shield]: https://img.shields.io/github/issues/coffeexcode/restfulqr.svg?style=for-the-badge
[issues-url]: https://github.com/coffeexcode/restfulqr/issues
[license-shield]: https://img.shields.io/github/license/coffeexcode/restfulqr.svg?style=for-the-badge
[license-url]: https://github.com/coffeexcode/restfulqr/blob/master/LICENSE.txt