import Select from "react-select/base";
import UploadFileBtn from "./UploadFileBtn";
import React, {useState} from "react";
import {SelectionOption} from "../models/selectionOption";
import {Group} from "../models/Group";


interface GroupSelectorProps {
    selectedValue: SelectionOption | null,
    setSelectedGroup: React.Dispatch<React.SetStateAction<SelectionOption | null>>
    options: SelectionOption[],
    searchValue: string,
    setSearchValue: React.Dispatch<React.SetStateAction<string>>

}


export const GroupSelector: React.FC<GroupSelectorProps> = ({
                                                                selectedValue,
                                                                setSelectedGroup,
                                                                options,
                                                                searchValue,
                                                                setSearchValue
                                                            }) => {
    const [menuIsOpen, setMenuIsOpen] = useState<boolean>()
    const handleChange = (selected: SelectionOption | null) => {
        setSelectedGroup(selected)
    }

    return (
        <Select
            styles={{
                control: (baseStyles, state) => ({
                    ...baseStyles,
                    background: "#262626",
                    color: "whitesmoke",
                    borderColor: state.isFocused ? 'whitesmoke' : '#3f3f46',
                }),
                menu: (base, props) => ({
                    ...base,
                    background: "#262626"
                }),
                option: (base, props) => ({
                    ...base,
                    backgroundColor: props.isFocused ? '#404040' : '262626',
                }),
                input: (base, props) => ({
                    ...base,
                    color: "whitesmoke"
                }),
                singleValue: (base, props) => ({
                    ...base,
                    color: "whitesmoke"
                })
            }}

            isSearchable={true}
            isClearable={true}
            value={selectedValue}
            onChange={handleChange}
            options={options}
            inputValue={searchValue}
            onInputChange={(newValue) => setSearchValue(newValue)}
            onMenuClose={() => setMenuIsOpen(false)}
            onMenuOpen={() => setMenuIsOpen(true)}
            menuIsOpen={menuIsOpen}
            placeholder={"Select group. . ."}/>

    )
}