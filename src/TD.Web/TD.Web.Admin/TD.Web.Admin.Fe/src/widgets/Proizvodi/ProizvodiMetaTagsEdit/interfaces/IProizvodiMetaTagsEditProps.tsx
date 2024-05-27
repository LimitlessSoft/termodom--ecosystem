export interface IProizvodiMetaTagsEditProps {
    metaTagTitle?: string,
    metaTagDescription?: string,
    onMetaTagTitleChange: (value?: string) => void,
    onMetaTagDescriptionChange: (value?: string) => void,
}