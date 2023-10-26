const express = require('express')
const router = express.Router()
const { getDb } = require('../db')
const multer = require('multer')
const path = require('path')
const fs = require('fs')

const software_releases_path = `${__basedir}/uploads/softwares`

const storage = multer.diskStorage({
    destination: (req, file, cb) => {
        cb(null, software_releases_path)
    },
    filename: (req, file, cb) => {
        getDb().collection('software').findOne({ _id: req.body.project_id }).then((r) => {
            cb(null, r.latest_version_path.substring(1))  
        }).catch((e) => {
            console.log(e)
            cb("Project with given id is not registered in database!")
        })
    }
})

const upload = multer({
    storage: storage,
    limits: {
        fieldSize: 104857600
    }
})

router.get('/info', async (req, res) => {
    getDb().collection('software').findOne({ _id: req.query.id }).then((r) => {
        return res.json({ id: r.id, title: r.title, last_version: r.last_version, minimal_version: r.minimal_version }).end()
    }).catch((e) => {
        console.log(e)
        return res.status(500).end()
    })
})

router.get('/download', async (req, res) => {
    getDb().collection('software').findOne({ _id: req.query.id }).then((r) => {
        return res.download(`${software_releases_path}${r.latest_version_path}`)
    })
})

router.post('/upload', upload.single('file'), async (req, res) => {
    if(req.file != null) {
        return res.status(201).end()
    }

    return res.status(500).end()
})

module.exports = router