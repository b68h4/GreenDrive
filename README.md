<div align="center">
  <a href="#">
    <picture>
      <source media="(prefers-color-scheme: dark)" height=256 width=256 srcset="https://github.com/b68h4/GreenDrive/blob/main/assets/appicon.png?raw=true">
      <img height=256 width=256 alt="GreenDrive" src="https://github.com/b68h4/GreenDrive/blob/main/assets/appicon.png?raw=true">
    </picture>
  </a>
  <h3>GreenDrive - A file explorer for Google Drive</h3>
  <hr/>
</div>

Greendrive is a lightweight web-based file manager. It allows users to share any file from Google Drive for download or display previews online through its integrated plugins.

## Features

-   For backend
    -   ✅ Rate limiting
    -   ✅ Firewall for domain (host,origin,referer) protection
    -   ✅ Range supported downloads
    -   ✅ Shared/Team drives
    -   ✅ Cache to reduce google api queries
-   For frontend
    -   ✅ Pdf reader
    -   ✅ Video player (supports music files)
    -   ✅ Fast navigation
    -   ✅ Download unpreviewiable files
    -   ✅ Popup ads for Your Telegram channel/group promotion
    -   ✅ Adsense advertisements (fixed placed)
    -   ✅ Gtag / Google analytics
    -   ✅ Simple configurable language definitions

## Prerequesties

-   .NET SDK 7.0 (macOS,Linux or Windows)
-   Node.js v16 or higher
-   For deploy (optional)
    -   Vercel cli
    -   Railway cli
-   Docker (optional) (for testing)

## Configuration

### Preparation

#### Get clientId and clientSecret from [Google Cloud Console](https://console.cloud.google.com/)

-   Go to the [Google Cloud Console](https://console.cloud.google.com/apis)
-   If you don't have a project, create one and then go to the **APIs & Services** tab.
-   From the library, enable the **Google Drive API**.
-   Then, from the **Credentials** section, create credentials for a **Desktop application**.
-   In the **OAuth consent screen** page, enter the following information:
    -   App name: Enter a name for your application.
    -   User type: Select the type of users that will be using your application.
    -   Scopes: Select the scopes that your application will need to access Google Drive.
-   Save the **clientId** and **clientSecret** somewhere safe.
<hr/>

-   **NOTE: Your Oauth application does not need to be in production mode, you can leave it in development mode.**

#### Clone repo to local

```console
foo@bar:~$ git clone https://github.com/b68h4/GreenDrive
```

### How can I get folder and shared drive id's from Google drive?

![](https://github.com/b68h4/GreenDrive/blob/main/assets/howtogetfolderid.gif)


### Backend configuration

-   appsettings.json (_For production use this_)
-   appsettings.Development.json

```json
  "GDrive": {
        "ClientId": "", // <- Put the clientId you received from Google cloud console here
        "ClientSecret": "ExampleClientSecret", // <- Put the clientSecret you received from Google cloud console here
        "EnableSharedDrive": false, // <- Set this to true if you want to enable the shared drive feature
        "EnableMainFolderCheck": true,
        "MainFolderId": "", // <- Put the Google Drive Id of the folder you want to share here
        "SharedDriveId": "", // <- If the shared drive feature is enabled, put the shared drive Id you want to share here.
        "AppName": "GreenDrive",
        "AuthFolder": "AuthCache"
    },
```

-   **NOTE: Please do not disable MainFolderCheck even though it may slow down api responses! If you disable it, people who somehow get the ID of another folder in your Google Drive can access it with simple hacks via GreenDrive.**

#### Deploy backend to railway (optional)

-   Go to the [Railway dashboard](https://railway.app/dashboard)

-   Click the "New Project" button and select "Empty Project".

-   Click the "New Service" button and select "Empty Service".

-   Go to your terminal and navigate to the backend folder:

```
cd backend
```

-   Run the command with the Railway CLI:

```
railway up -s <yourservice>
```

-   Once the deployment is complete, create a domain for your service through Railway

    -   Click the "Domains" tab and click the "Create Domain" button.

-   Save the domain somewhere safe

#### Google authorization

Backend will give you a token url path for verification on the first run, you can see it locally in the console or in the logs of other providers.

It should look like this:

```
http://{yourdomain}/Api/Auth?token=XXXYYYZZZ
```

Let's assume you deployed the backend to **Railway**.

Your domain is probably like this:

```
https://x-y-z-1234.up.railway.app/
```

Concatenate the URL path you received from the console and make it like this:

```
https://x-y-z-1234.up.railway.app/Api/Auth?token=XXXYYYZZ
```

When you open this url in your browser, it will direct you to the google login page. Give the necessary permissions for google drive on this page and complete the login. Then, if the process is successful, it will take you to the `/Api/List` url. If you see the contents of the folder you specified as mainFolderId in json format, congratulations, you have completed authorization.

### Frontend configuration

-   config.json

```json
{
    "appName": "GreenDrive",
    "apiUrl": "http://localhost:8081", // (Required) Put the backend api url to here without /
    "language": "en_us", // You can select file explorer language here
    "tg_ads": ... // You can configure telegram ads popup here
    "adsense": ... // You can configure adsense advertisements here
    "gtag": ... // // You can configure gtag.js here
    "language_strings": ... // You can modify or define languages here
}

```

#### Deploy frontend to vercel (optional)

-   Navigate to the frontend folder
-   Run the Vercel CLI with the vercel command and follow the instructions to deploy your application

```
cd frontend
vercel
```

_Your project should now be accessible from the domain of the platform you deployed or via localhost._

## About of the project

_This project was developed years ago for a small telegram channel, but is now open source in its refactored form._

-   First development year: 2020-2021 (known as CoderatorDepo)
-   Refactoring year: 2024

## License

This project uses the GNU Affero General Public License Version 3 (GNU-AGPL-3.0)

## Disclaimer

The developer disclaims any responsibility or liability for the use of this project involving illegal content, copyright infringement, or any unlawful activity. Users are solely responsible for ensuring compliance with all applicable laws and regulations.

Furthermore, the developer shall not be held liable for any damages, losses, or consequences arising from the use or misuse of this project. This project is provided on an "as is" and "as available" basis, without any warranties, express or implied.
