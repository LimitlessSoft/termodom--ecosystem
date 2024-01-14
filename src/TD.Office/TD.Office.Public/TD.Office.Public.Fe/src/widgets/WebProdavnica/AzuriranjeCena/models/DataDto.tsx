export interface DataDto {
    id: number;
    naziv: string;
    minWebOsnova: number;
    maxWebOsnova: number;
    nabavnaCenaKomercijalno: number;
    prodajnaCenaKomercijalno: number;
    ironCena: number;
    silverCena: number;
    goldCena: number;
    platinumCena: number;
    linkRobaId: number | null;
    linkId?: number;
    uslovFormiranjaWebCeneModifikator: number;
    uslovFormiranjaWebCeneId: number;
    uslovFormiranjaWebCeneType: number;
}