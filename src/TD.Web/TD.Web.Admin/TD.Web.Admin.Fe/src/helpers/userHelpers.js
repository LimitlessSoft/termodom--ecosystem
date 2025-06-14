import { mainTheme } from '../theme'

export const getUserTrackPriceLevelColor = (trackPriceLevel, customColors) => {
    switch (trackPriceLevel) {
        case 1:
            return (
                customColors?.doNotTrack ??
                mainTheme.palette.primary.contrastText
            )
        case 2:
            return customColors?.lowLevelTrack ?? `#fbbdbd`
        case 0:
        default:
            return customColors?.track ?? mainTheme.palette.error.dark
    }
}
