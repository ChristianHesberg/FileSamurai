import Select from "react-select/base";
import UploadFileBtn from "./UploadFileBtn";
import React, {useState} from "react";
import {SelectionOption} from "../models/selectionOption";
import {Group} from "../models/Group";
import {SelectorStyle} from "../styles/SelectorStyle";
import {isDisabled} from "@testing-library/user-event/dist/utils";

interface selectorProps {
    selectedValue: SelectionOption | null,
    onChange: any,
    options: SelectionOption[],
    searchValue: string,
    setSearchValue: React.Dispatch<React.SetStateAction<string>>
    isMulti?: boolean
    isDisabled?: boolean,
    placeholder?: string

}


export const Selector: React.FC<selectorProps> = ({
                                                      selectedValue,
                                                      onChange,
                                                      options,
                                                      searchValue,
                                                      setSearchValue, isMulti = false
                                                      , isDisabled = false,
                                                      placeholder
                                                  }) => {
    const [menuIsOpen, setMenuIsOpen] = useState<boolean>()


    return (
        <Select
            isDisabled={isDisabled}
            styles={SelectorStyle}
            isMulti={isMulti}
            isSearchable={true}
            isClearable={true}
            value={selectedValue}
            onChange={onChange}
            options={options}
            inputValue={searchValue}
            onInputChange={(newValue) => setSearchValue(newValue)}
            onMenuClose={() => setMenuIsOpen(false)}
            onMenuOpen={() => setMenuIsOpen(true)}
            menuIsOpen={menuIsOpen}
            placeholder={placeholder}/>

    )
}