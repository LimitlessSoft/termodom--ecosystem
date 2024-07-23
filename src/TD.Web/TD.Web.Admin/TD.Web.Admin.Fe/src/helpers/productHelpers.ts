export const getStatuses = (): { [key: string]: number } => {
    return {
        vidljiv: 0,
        NoviNaObradi: 1,
        NoviCekaOdobrenje: 2,
        AzuriranjeNaObradi: 3,
        AzuriranjeCekaOdobrenje: 4,
        Sakriven: 5,
    }
}
