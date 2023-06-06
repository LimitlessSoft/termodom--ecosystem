Application Port: `43100`

## Building docker iamge
`docker build . -t limitlesssoft/td-dev-api:[version(1.0.0)]`

## Running docker container
`docker run -d --name "container name" -p [Host Port]:[Application Port] limitlesssoft/td-dev-api:[version(1.0.0)]`

## MongoDB Schema
```
DB Name: td-dev-api

Collections: [
    software
]
```

## Manaaging softwares info
Import software data as document inside software collection in given format:
```
{
  "_id": "SoftwareID",
  "title": "SoftwareTitle",
  "last_version": "Version",
  "minimal_version": "Version",
  "latest_version_path": "/Archive.rar/Executable.exe" // prefix / is required
}
```

## Endpoints
```
// Gets info about software (title, last_version, minimal_version)
[GET]> /software/info?id={softwareID}

// Uploads release files onto server. Upload destination path = {__root}/uploads/software/[SoftwareID]
[POST]> /software/upload
[Body]> { project_id, file }

// Downloads release files from the server for the given project from the path
// {__root}/uploads/software/[latest_version_path from database for given softwareID]
[GET]> /software/download?id={softwareID}
```
