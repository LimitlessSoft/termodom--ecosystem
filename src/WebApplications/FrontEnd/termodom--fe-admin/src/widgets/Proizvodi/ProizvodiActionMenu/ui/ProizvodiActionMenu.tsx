import { HeaderActionMenu, IAction } from "@/widgets/HeaderActionMenu"

export const ProizvodiActionMenu = (): JSX.Element => {
    return (
        <HeaderActionMenu actions={[
            {
                callback: () => {
                    alert("hi")
                },
                text: "Novi"
            }
        ]} />
    )
}