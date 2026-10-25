export type EquipmentItems = 'Projector' | 'TVscreen' | 'VideoConferencing' | 'Whiteboard'

export type Room = {
    name: string
    id: number
    capacity: number
    floor: string
    equipment: EquipmentItems[]
    isActive: boolean
}