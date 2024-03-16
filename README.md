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

Greendrive is a lightweight, web-based file manager. It allows users to access, download and preview any file from Google Drive online.

### Demo live on: [GreenDrive - Demo](https://greendrive-demo.vercel.app/)

## Features

-   Backend
    -   ✅ Rate limiting
    -   ✅ Firewall for domain (host,origin,referer) protection
    -   ✅ Resumable downloads
    -   ✅ Shared/Team drives
    -   ✅ Cache to reduce Google API queries
-   Frontend
    -   ✅ PDF reader
    -   ✅ Video player (supports music files)
    -   ✅ Fast navigation
    -   ✅ Download unpreviewable files
    -   ✅ Pop-up ads for Telegram channel/group promotion
    -   ✅ Adsense advertisements (fixed placed)
    -   ✅ Gtag / Google analytics
    -   ✅ Simple configurable language definitions

## Prerequirements

-   .NET SDK 7.0 (macOS,Linux or Windows)
-   Node.js v16 or higher
-   To deploy (optional)
    -   Vercel cli
    -   Railway cli
-   Docker (optional, for testing)

## Configuration

### Preparation

#### Get clientId and clientSecret from [Google Cloud Console](https://console.cloud.google.com/)

-   Go to [Google Cloud Console](https://console.cloud.google.com/apis)
-   If you don't have a project, create one and then go to the **APIs & Services** tab.
-   From the library, enable the **Google Drive API**.
-   Then, from the **Credentials** section, create credentials for a **Web application**.
-   After selecting the Web application option, you should see a field labeled Authorized redirect URIs.
    -   Add the backend URI to that field: `https://{yourdomain}/Api/Auth/Callback` (ex: `https://yourdeployment-1234.up.railway.app/Api/Auth/Callback`)
-   In the **OAuth consent screen** page, enter the following information:
    -   App name: Enter a name for your application.
    -   User type: Select the type of users who use your application.
    -   Scopes: Select the scopes that your application needs to access Google Drive.
-   Save **clientId** and **clientSecret** somewhere safe.
<hr/>

-   **NOTE: Your OAuth application does not need to be in production mode, you can leave it in development mode.**

#### Clone This Repo

```console
foo@bar:~$ git clone https://github.com/b68h4/GreenDrive
```

### How Can I Get Folder and Shared Drive Id’s From Google Drive?

![](https://github.com/b68h4/GreenDrive/blob/main/assets/howtogetfolderid.gif)

### Backend Configuration

-   appsettings.json (_In production_)
-   appsettings.Development.json

```json
  "GDrive": {
        "ClientId": "", // <- Enter the clientId you received from Google Cloud Console here
        "ClientSecret": "ExampleClientSecret", // <- Enter the clientSecret you received from Google Cloud Console here
        "EnableSharedDrive": false, // <- Set to true if you want to enable the shared drive feature
        "EnableMainFolderCheck": true,
        "MainFolderId": "", // <- Enter the Google Drive ID of the folder you want to share
        "SharedDriveId": "", // <- If the shared drive feature is enabled, enter the shared drive ID you want to share here
        "AppName": "GreenDrive",
        "AuthFolder": "AuthCache"
    }
```

-   **NOTE: Please do not disable MainFolderCheck, even though it may slow down API responses! If you disable it, people who somehow got the ID of another folder in your Google Drive will be able to access it with simple hacks via GreenDrive.**

#### Deploy Backend to Railway (Optional)

-   Go to [Railway dashboard](https://railway.app/dashboard)

-   Click the "New Project" button and select "Empty Project".

-   Click the "New Service" button and select "Empty Service".

-   Go to your terminal and navigate to the backend folder:

```
cd backend
```

-   Run these commands:

```
railway link
railway up -s <yourservice>
```

-   Once the deployment is complete, create a domain for your service in Railway

    -   Click the "Domains" tab and click the "Create Domain" button.

-   Save the domain somewhere safe

#### Google Authorization

Backend will give you a url path with a token in it for verification on the first start, you can see it locally in the console or in the logs of other providers.

It should look like this:

```
http://{yourdomain}/Api/Auth?token=XXXYYYZZZ
```

Let's assume that you have deployed the backend to **Railway**.

Your domain probably looks like this:

```
https://x-y-z-1234.up.railway.app/
```

Concatenate the URL path you got from the console and make it look like this:

```
https://x-y-z-1234.up.railway.app/Api/Auth?token=XXXYYYZZ
```

Open this URL in your browser, it will take you to the Google sign-in page. On this page, grant the necessary Google Drive permissions and complete the sign-in. If the process is successful, you will then be redirected to the `/Api/List` URL. If you see the contents of the folder you specified as mainFolderId in the configuration, congratulations, you have completed the authorization.

### Frontend Configuration

-   config.json

```json
{
    "appName": "GreenDrive",
    "apiUrl": "http://localhost:8081", // (Required) Enter the backend API URL without the trailing slash here
    "language": "en_us", // Select file explorer language here
    "tg_ads": ... // Configure Telegram ad pop-up here
    "adsense": ... // Configure adsense advertisements here
    "gtag": ... // // Configure gtag.js here
    "language_strings": ... // Modify or define languages here
}

```

#### Deploy Frontend to Vercel (Optional)

-   Navigate to the frontend folder
-   Run the Vercel CLI and follow the instructions to deploy your application

```
cd frontend
vercel
```

_Your project should now be accessible from the domain of the platform you deployed or from localhost._

## About the project

_This project was developed years ago for a small telegram channel, but is now open source in its refactored form._

-   First development year: 2020-2021 (known as CoderatorDepo)
-   Refactoring year: 2024

## License

This project uses the GNU Affero General Public License Version 3 (GNU-AGPL-3.0)

## Disclaimer

The developer disclaims any responsibility or liability for the use of this project involving illegal content, copyright infringement, or any unlawful activity. Users are solely responsible for ensuring compliance with all applicable laws and regulations.

Furthermore, the developer shall not be held liable for any damages, losses, or consequences arising from the use or misuse of this project. This project is provided on an "as is" and "as available" basis, without any warranties, express or implied.
