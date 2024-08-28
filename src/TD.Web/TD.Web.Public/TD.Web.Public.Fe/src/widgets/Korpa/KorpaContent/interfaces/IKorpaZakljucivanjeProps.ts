export interface IKorpaZakljucivanjeProps {
    oneTimeHash?: string
    paymentTypeId: number
    favoriteStoreId: number
    onSuccess: () => void
    onProcessStart: () => void
    onProcessEnd: () => void
    onFail: () => void
}
