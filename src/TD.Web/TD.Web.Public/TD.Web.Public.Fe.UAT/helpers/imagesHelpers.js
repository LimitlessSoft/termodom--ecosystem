import { BUFFER, PUBLIC_API_CLIENT } from '../constants.js'
import MinioClient from 'td-common-minio-node'
import { vaultClient } from '../configs/vaultConfig.js'
import MemoryStream from 'memorystream'
import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const { MINIO_ACCESS_KEY, MINIO_SECRET_KEY, MINIO_PORT, MINIO_HOST } =
    await vaultClient.getSecret('web/public/api')

const minioClient = new MinioClient({
    accessKey: MINIO_ACCESS_KEY,
    secretKey: MINIO_SECRET_KEY,
    port: MINIO_PORT,
    endPoint: MINIO_HOST,
})

const imagesHelpers = {
    async fetchImages() {
        const imagePromises = Array.from({ length: 10 }, async () => {
            const imageUrl = `https://picsum.photos/${faker.number.int({
                min: 300,
                max: 600,
            })}/${faker.number.int({ min: 300, max: 600 })}`
            const response = await PUBLIC_API_CLIENT.axios.get(imageUrl, {
                responseType: 'arraybuffer',
            })
            const memoryStream = new MemoryStream()
            memoryStream.write(response.data)
            memoryStream.end()

            return {
                stream: memoryStream,
                metadata: {
                    'Content-Type': response.headers['content-type'],
                },
            }
        })

        return Promise.all(imagePromises)
    },
    generateSimpleFilename() {
        const timestamp = Date.now()
        const randomNum = Math.floor(Math.random() * 1000)
        return `image_${timestamp}_${randomNum}`
    },
    getRandomImageStream() {
        if (!BUFFER.IMAGES.length) {
            throw new Error('No pre-fetched images available.')
        }

        const randomImage =
            BUFFER.IMAGES[Math.floor(Math.random() * BUFFER.IMAGES.length)]

        return {
            stream: randomImage.stream,
            filename: this.generateSimpleFilename(),
            metadata: randomImage.metadata,
        }
    },
    async uploadImageToMinio() {
        const { stream, filename, metadata } =
            imagesHelpers.getRandomImageStream()

        await minioClient
            .uploadObject(
                'automation.td.web',
                `images/${filename}`,
                stream,
                metadata
            )
            .catch((error) => {
                throw new Error(`Error uploading object: ${error.message}`)
            })

        return filename
    },
    async removeImageFromMinio(filename) {
        await minioClient
            .removeObject('automation.td.web', `images/${filename}`)
            .catch((error) => {
                throw new Error(`Error removing object: ${error.message}`)
            })
    },
}

export default imagesHelpers
