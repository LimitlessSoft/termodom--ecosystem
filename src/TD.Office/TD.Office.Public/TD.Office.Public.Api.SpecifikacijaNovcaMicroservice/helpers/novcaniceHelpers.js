function novcanicaFind(key) {
    return this.find((novcanica) => novcanica.key === key).value
}

Array.prototype.novcanicaFind = novcanicaFind
