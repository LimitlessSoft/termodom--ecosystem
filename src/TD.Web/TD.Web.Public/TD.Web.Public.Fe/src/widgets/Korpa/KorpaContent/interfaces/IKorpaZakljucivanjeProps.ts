export interface IKorpaZakljucivanjeProps {
    oneTimeHash?: string,
    favoriteStoreId: number,
    onSuccess: () => void,
    onProcessStart: () => void,
    onProcessEnd: () => void,
    onFail: () => void
}