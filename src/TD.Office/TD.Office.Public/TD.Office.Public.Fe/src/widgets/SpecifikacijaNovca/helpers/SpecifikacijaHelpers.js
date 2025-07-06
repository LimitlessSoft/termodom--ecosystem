export const getUkupnoGotovine = (specifikacijaNovca) =>
    specifikacijaNovca?.specifikacijaNovca.novcanice.reduce(
        (prevNovcanica, currentNovcanica) =>
            prevNovcanica + currentNovcanica.value * currentNovcanica.key,
        0
    ) ?? 0
