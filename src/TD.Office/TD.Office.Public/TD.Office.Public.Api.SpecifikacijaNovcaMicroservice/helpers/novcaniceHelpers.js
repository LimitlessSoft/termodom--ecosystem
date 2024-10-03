function novcanicaFind(key) {
    return this.find((novcanica) => novcanica.key === key)
}

Array.prototype.novcanicaFind = novcanicaFind
