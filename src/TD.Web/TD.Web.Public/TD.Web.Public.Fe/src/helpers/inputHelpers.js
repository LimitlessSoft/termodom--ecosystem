export function blockNonDigitKeys(e) {
    const blockedKeys = ['e', 'E', '+', '-', '.']
    if (blockedKeys.includes(e.key)) {
        e.preventDefault()
    }
}
