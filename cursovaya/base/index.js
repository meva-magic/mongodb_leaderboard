require('dotenv').config();

const express = require("express");
const connectDB = require("./config/db");
const Leaderboard = require("./models/LeaderboardSchema")
const app = express();

connectDB();
app.use(express.json());
app.use(express.urlencoded({extended: true}));

const port = process.env.PORT || 3009;


app.post("/api/leaderboard", async (req, res) => 
{
    const {name, score} = req.body;

    if(!name || !score)
    {return res.status(400).json({msg: "Please enter all fields"})}

    try
    {
        const newScore = new Leaderboard
        ({name, score,});

        const scoreSaved = await newScore.save();
        res.json(scoreSaved);
    }

    catch (error)
    {
        console.log(error);
        res.status(500).json({msg: "Server Error"});
    }
});


app.get("/api/leaderboard", async (req, res) => 
{
    try
    {
        const scores = await Leaderboard.find().sort({score: -1}).limit(13);
        res.json(scores);
    }

    catch (error)
    {
        console.log(error);
        res.status(500).json({msg: "Server Error"});
    }
});


app.listen(port, () => 
{
    console.log(`Server is running on port ${port}`);
});
