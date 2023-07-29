export default function Button({ onClick, text, disabled, additionalClasses, showSpin }: any) {
    return (
        <button
            disabled={disabled}
            className={`td-button border-solid bg-td-red text-white font-medium px-3 py-1 m-1 duration-75 ${additionalClasses}`}
            onClick={(event) => {
                onClick(event)
            }}>
                {text}
            </button>
    )
}