const Minio = require('minio')

class MinioManager {
    #client

    constructor({ accessKey, secretKey, port, endPoint }) {
        this.#client = new Minio.Client({
            endPoint: endPoint,
            port: port,
            accessKey: accessKey,
            secretKey: secretKey,
            useSSL: false,
        })
    }

    async uploadObject(bucketName, objectName, stream, metadata) {
        return new Promise((resolve, reject) => {
            this.#client.putObject(
                bucketName,
                objectName,
                stream,
                null,
                metadata,
                (err, objInfo) => {
                    if (err) return reject(err)
                    resolve(objInfo)
                }
            )
        })
    }

    async removeObject(bucketName, objectName) {
        return new Promise((resolve, reject) => {
            this.#client.removeObject(bucketName, objectName, (err) => {
                if (err) return reject(err)
                resolve()
            })
        })
    }
}

module.exports = {
    generateMinioClient: async ({ accessKey, secretKey, port, endPoint }) => {
        const minioClient = new MinioManager({
            accessKey,
            secretKey,
            port,
            endPoint,
        })
        return minioClient
    },
}
