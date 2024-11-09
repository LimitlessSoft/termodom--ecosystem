import { mainTheme } from '../../themes'

export const PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS = {
    ALIGNMENT: 'center',
    KOMERCIJALNO: 'komercijalno',
    FINANSIJSKO_KUPAC: 'finansijskoKupac',
    FINANSIJSKO_DOBAVLJAC: 'finansijskoDobavljac',
    TABLE_HEAD_FIELDS: {
        NAZIV: 'Naziv',
        PPID: 'PPID',
        POCETAK_SUFFIX: 'Pocetak',
        KRAJ_SUFFIX: 'Kraj',
    },
    SHOW_DATA_LABEL: 'Prikaži podatke',
    COLUMN_TABLE_WIDTH: 250,
    INITIAL_PAGE: 1,
    INITIAL_PAGE_SIZE: 500,
    CELL_DATA_TYPE_CHIP_BG_COLOR_KOMERCIJALNO:
        mainTheme.palette.action.selected,
    CELL_DATA_TYPE_CHIP_BG_COLOR_FINANSIJSKO: mainTheme.palette.info.light,
}
