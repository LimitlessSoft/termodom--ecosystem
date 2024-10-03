function sum(cb) {
    return this.reduce((prev, curr) => prev + (cb ? cb(curr) : curr), 0)
}

Array.prototype.sum = sum
