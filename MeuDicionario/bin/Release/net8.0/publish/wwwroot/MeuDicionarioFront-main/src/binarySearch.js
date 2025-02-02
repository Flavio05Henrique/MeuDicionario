export const binarySearch = (list, val) => {
    let init = 0
    let end = list.length -1

    while(init <= end) {
        const middle = parseInt((init + end) / 2)
        const test = list[middle]
        
        if(test.id == val) return {'element': test, 'index': middle}
        test.id > val ? init = middle + 1 : end = middle -1
    }

    return null
}