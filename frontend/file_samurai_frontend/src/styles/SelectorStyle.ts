export const SelectorStyle:any ={
    control: (baseStyles:any, state :any) => ({
        ...baseStyles,
        background: "#262626",
        color: "whitesmoke",
        borderColor: state.isFocused ? 'whitesmoke' : '#3f3f46',
    }),
        menu: (baseStyles:any, state :any) => ({
        ...baseStyles,
        background: "#262626"
    }),
        option: (baseStyles:any, state :any) => ({
        ...baseStyles,
        backgroundColor: state.isFocused ? '#404040' : '262626',
    }),
        input: (baseStyles:any, state :any) => ({
        ...baseStyles,
        color: "whitesmoke"
    }),
        singleValue: (baseStyles:any, state :any) => ({
        ...baseStyles,
        color: "whitesmoke"
    })
}