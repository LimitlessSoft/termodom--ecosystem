export interface IProductGroup {
    id: number
    name: string
    parentGroupId: number | null
    welcomeMessage: string | null
    typeId: number
}
