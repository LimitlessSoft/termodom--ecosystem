export interface IProizvodiMetaTagsEditProps {
    disabled: boolean
    metaTagTitle?: string
    metaTagDescription?: string
    onMetaTagTitleChange: (value?: string) => void
    onMetaTagDescriptionChange: (value?: string) => void
}
