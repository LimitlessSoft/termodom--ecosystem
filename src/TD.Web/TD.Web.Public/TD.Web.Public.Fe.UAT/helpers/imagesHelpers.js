import {
    BUFFER,
    MINIO_PORT,
    MINIO_HOST,
    PUBLIC_API_CLIENT,
    MINIO_ACCESS_KEY,
    MINIO_SECRET_KEY,
} from '../constants.js'
import { generateMinioClient } from 'td-common-minio-node'
import { Readable } from 'stream'
// import MemoryStream from 'memorystream'
// const { Readable } = MemoryStream

const minioClient = await generateMinioClient({
    accessKey: MINIO_ACCESS_KEY,
    secretKey: MINIO_SECRET_KEY,
    port: MINIO_PORT,
    endPoint: MINIO_HOST,
})

const imagesHelpers = {
    generateSimpleFilename: () => {
        const timestamp = Date.now()
        const randomNum = Math.floor(Math.random() * 1000)
        return `images/image_${timestamp}_${randomNum}`
    },
    getRandomImageStream: async () => {
        try {
            const randomImageUrl =
                BUFFER.images[Math.floor(Math.random() * BUFFER.images.length)]
            const response = await PUBLIC_API_CLIENT.axios.get(randomImageUrl, {
                responseType: 'arraybuffer',
            })

            const memoryStream = new Readable()
            memoryStream._read = () => {}
            memoryStream.push(response.data)
            memoryStream.push(null)

            return {
                stream: memoryStream,
                filename: imagesHelpers.generateSimpleFilename(),
                metadata: {
                    'Content-Type': response.headers['content-type'],
                },
            }
        } catch (error) {
            console.error('Error fetching random image stream:', error)
            throw error
        }
    },
    uploadImageToMinio: async () => {
        try {
            const { stream, filename, metadata } =
                await imagesHelpers.getRandomImageStream()

            await minioClient
                .uploadObject('automation.td.web', filename, stream, metadata)
                .then((data) => console.log(data))
                .catch((error) =>
                    console.error('Error uploading object:', error)
                )

            return filename
        } catch (error) {
            console.error('Error uploading image:', error)
            throw error
        }
    },
    removeImageFromMinio: async (filename) => {
        try {
            await minioClient
                .removeObject('automation.td.web', filename)
                .then(() =>
                    console.log(`Successfully removed image: ${filename}`)
                )
                .catch((error) =>
                    console.error('Error removing object:', error)
                )
        } catch (error) {
            console.error('Error removing image:', error)
            throw error
        }
    },
}

export default imagesHelpers
