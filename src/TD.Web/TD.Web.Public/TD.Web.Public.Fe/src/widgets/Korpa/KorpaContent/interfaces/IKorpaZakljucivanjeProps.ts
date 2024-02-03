export interface IKorpaZakljucivanjeProps {
    oneTimeHash?: string,
    onSuccess: () => void,
    onProcessStart: () => void,
    onProcessEnd: () => void,
}