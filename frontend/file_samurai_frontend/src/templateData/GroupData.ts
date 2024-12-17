import {Group} from "../models/Group";


const g1: Group = {
    name: "my cool group",
    members: [{id: "1", email: "Cool@gmail.com"}],
    id: "Filler id 1"
}
const g2: Group = {
    name: "The lame group",
    members: [{id: "2", email: "JimmyNice@gmail.com"}, {id: "3", email: "OleRam@gmail.com"}],
    id: "Filler id 2"
}


export default function GetGroups(): Group[] {
    return [g1, g2]
}