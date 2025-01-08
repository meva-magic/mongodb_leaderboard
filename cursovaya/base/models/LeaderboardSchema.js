const mongoose = require("mongoose");

const LeaderboardSchema = new mongoose.Schema
({
    name:
    {
        type: String,
        required: true,
    },

    score:
    {
        type: Number,
        required: true,
    },

    date:
    {
        type: Date,
        default: Date.now,
    },
});

module.exports = mongoose.model("Leaderboard", LeaderboardSchema);