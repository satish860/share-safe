# share-safe
[![.NET](https://github.com/satish860/share-safe/actions/workflows/dotnet.yml/badge.svg)](https://github.com/satish860/share-safe/actions/workflows/dotnet.yml)

Easy and secure way to store and share any files. 

- `createFile - HTTP POST /files/` (creates a file object with metadata, but not the file yet)
- `listFiles - HTTP GET /files/` (lists all the file objects that are created)
- `uploadFile - HTTP POST /files/{fileId}/upload` (uploads a file)
- `downloadFile - HTTP GET /files/{fileId}/download` (downloads a file)
- `deleteFileByID - HTTP DEL /files/{fileId}` (deletes a file object and content)

# CreateFile - Done

- Just creates a Metadata of the file and Return the UUID for the reference
- Use MongoDB for creating the File

## UploadFile

- Should be able to Upload the file to Digital Ocean space.
- Research if we can add the ID in the metadata. What if we use the ID as a Key?
- So I can query by Metadata instead of using ID rather than going to the database

## DownloadFile :

should be able to download files by ID.

# Delete File: - Done

Delete File By ID

## List Files - Done

- This should be Getting all the files from Bucket and display
