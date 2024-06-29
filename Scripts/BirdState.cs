public enum BirdState{
    None = -1,
    Idle = 0,
    ApproachOwner, //This is for new followers to approach the owner
    FollowOwner, // This is for current followers who are out of the area to turn to the owner.

    Loiter, // Loitering around the player.
    Attack, // Basic attack command
    Defend, // Defend command

    Dead,

    Count
}
