export const PERMISSIONS_CONSTANTS = {
    PERMISSIONS_GROUPS: {
        NAV_BAR: 'nav-bar',
        PRODUCTS: 'products',
    },
    USER_PERMISSIONS: {
        PROIZVODI: {
            READ: 'Admin_Products_Access',
            EDIT_ALL: 'Admin_Products_EditAll',
            EDIT_SRC: 'Admin_Products_EditSrc',
            EDIT_META_TAGS: 'Admin_Products_EditMetaTags',
        },
        PORUDZBINE: { READ: 'Admin_Orders_Access' },
        KORISNICI: {
            READ: 'Admin_Users_Access',
            WRITE: 'Admin_Users_Write',
        },
        PODESAVANJA: { READ: 'Admin_Settings_Access' },
        BLOGOVI: {
            READ: 'Admin_Blogs_Access',
            WRITE: 'Admin_Blogs_Write',
        },
    },
}
