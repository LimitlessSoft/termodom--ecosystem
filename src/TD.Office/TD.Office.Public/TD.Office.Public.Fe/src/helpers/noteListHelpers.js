const noteListPrefix = 'noteList-dwabcgwa89cbgaw9cgawb78vwg:'
export const isNoteList = (jsonString) => {
    if (typeof jsonString !== 'string') return false

    if (jsonString.startsWith('noteList-dwabcgwa89cbgaw9cgawb78vwg:'))
        return true

    return false
}

export const parseNoteList = (jsonString) => {
    if (!isNoteList(jsonString)) return []

    const json = jsonString.replace(noteListPrefix, '')
    try {
        return JSON.parse(json)
    } catch (e) {
        console.error('Failed to parse note list:', e)
        return []
    }
}

export const stringifyNoteList = (noteList) => {
    if (!Array.isArray(noteList)) {
        console.error('Expected an array for note list')
        return ''
    }

    try {
        const json = JSON.stringify(noteList)
        return noteListPrefix + json
    } catch (e) {
        console.error('Failed to stringify note list:', e)
        return ''
    }
}
