## Inspiration
Who hasn't ever felt stressed and wanted to listen to peaceful music? Or perhaps felt really excited and wanted to have epic music ful-surround and imagine to be driving in the middle of a Highlands-like landscape? Well, wouldn't it be cool to turn on the laptop, ask him for music and immediately have the music you want without having to ask him to?

## What it does
**Moodsic** reads your face using a camera (eg your laptops webcam) and learns your mood. Then it uses this info to pick an appropriate music for your mood.

## How we built it
We used **Microsoft's Cognitive API*** for the facial recognition and a .NET environment for the development (C#, XMAL, WPF). For the music selection we used **Rhythm API** by Gracenote Developer. Finally, the songs are directly downloaded from YouTube.

## Challenges we ran into
- Lack of an official/universal implementation to use the webcam.
- Difficulties to get audios/videos from songs.
- Using WPF environment, which lacks of community support compared to older environments.

## Accomplishments that we're proud of
- Most important, overcoming the challenges mentioned above.
- Developing with an environment which was completely new to all of us.

## What we learned
- Exciting and useful APIs.
- New programming environments (I'm sorry if I repeat myself too much, I'm sorry).

## What's next for Moodsic
Ordered from easier to harder to implement:
Allow the user to choose the genre, era, or artist they want to listen to if they want something more specific.
Use a personal library of music combined with Internet-retrieved music.
Improve the parallelism of the application for the downloads to avoid making the user wait.
Use ML to better predict what the user wants to listen to. We've thought of a neuronal network with user's mood as input and the track's _mood_ and genre as outputs.
Improve the UI. (sorry not fans of front-end)
