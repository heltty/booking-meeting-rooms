export type ReservationStatus = 'Booked' | 'Completed' | 'Cancelled'

export type Reservation = {
    id: number
    roomId: number
    organizerId: number
    meetingTitle: string
    start: string
    end: string
    attendees: number
    status: ReservationStatus
}