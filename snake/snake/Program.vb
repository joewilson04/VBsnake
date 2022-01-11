Imports System

Module Program
    Dim worm(99, 99) As Integer         'tracks where the worm is
    Dim arena(49, 23) As Char           'used to create bounds
    Dim highScoreName(9) As String      'leaderboard names
    Dim highScoreValue(9) As Integer    'leaderboard scores
    Dim wormHeadPointer As Integer      'used to record the current head of the worm in the worm array
    Dim wormTailPointer As Integer      'used to record the current tail of the worm in the worm array
    Dim wormDirection As Char           'direction the worm is moving: u,d,l,r
    Dim wormAlive As Boolean            'whether the worm is alive or dead flag
    Dim wormGrow As Boolean             'whether the worm needs to grow a segment or not
    Dim gameSpeed As Integer            'game delay in milliseconds
    Dim wormSpeedX As Integer           'game delay in milliseconds
    Dim wormSpeedY As Integer           'game delay in milliseconds
    Dim score As Integer                'current score
    Dim foodTimer As Integer            'bonus score counts down until eaten or zero

    Const forever As Boolean = False    'allows the game to loop forever
    Sub Main(args As String())

        initialiseHighScores()
        Do
            initialiseGame()
            arenaDisplay()
            Do
                System.Threading.Thread.Sleep(gameSpeed)
                getInput()
                moveWormX()
                getInput()
                updateFoodTimer()
            Loop Until (wormAlive = False)
            gameEnd()
            highScores()
        Loop Until (forever)
    End Sub

    Sub initialiseGame()
        Console.BackgroundColor = ConsoleColor.Red
        Dim counterX, counterY As Integer
        'initialise arena
        For counterX = 0 To 49
            For counterY = 0 To 23
                arena(counterX, counterY) = " "
            Next
        Next
        'initialise game variables
        gameSpeed = 100  'the lower the number, the faster the snake
        score = 0
        wormAlive = True
        wormGrow = False
        wormHeadPointer = 4
        wormTailPointer = 1
        wormDirection = "r"
        worm(1, 1) = 5
        worm(1, 2) = 5
        arena(5, 5) = "#"
        worm(2, 1) = 6
        worm(2, 2) = 5
        arena(6, 5) = "#"
        worm(3, 1) = 7
        worm(3, 2) = 5
        arena(7, 5) = "#"
        worm(4, 1) = 8
        worm(4, 2) = 5
        arena(8, 5) = "#"
        Randomize()
        spawnFood()

    End Sub

    Sub arenaDisplay()
        Dim counterX, counterY As Integer
        Console.CursorVisible = False
        Console.Clear()
        Console.SetCursorPosition(55, 2)
        Console.Write("Score: ", score)
        Console.SetCursorPosition(55, 4)
        Console.Write("Bonus: 200")
        For counterX = 0 To 49
            For counterY = 0 To 23
                Console.SetCursorPosition(counterX, counterY)
                Console.Write(arena(counterX, counterY))
            Next
        Next

    End Sub

    Sub moveWormX()
        Dim wormHeadX As Integer
        Dim wormHeadY As Integer
        'calculate new head position
        wormHeadX = worm(wormHeadPointer, 1)
        wormHeadY = worm(wormHeadPointer, 2)
        wormHeadPointer = wormHeadPointer + 1
        If (wormHeadPointer = 100) Then wormHeadPointer = 0
        Select Case wormDirection
            Case "r"
                wormHeadX = wormHeadX + 1
            Case "l"
                wormHeadX = wormHeadX - 1
            Case "u"
                wormHeadY = wormHeadY - 1
            Case "d"
                wormHeadY = wormHeadY + 1
        End Select

        ' wrap worm around screen if needed
        If (wormHeadX = 50) Then wormHeadX = 0
        If (wormHeadX = -1) Then wormHeadX = 49
        If (wormHeadY = -1) Then wormHeadY = 23
        If (wormHeadY = 24) Then wormHeadY = 0

        'set new worm head position
        worm(wormHeadPointer, 1) = wormHeadX
        worm(wormHeadPointer, 2) = wormHeadY
        collisionDetectionAndScore()
        arena(wormHeadX, wormHeadY) = "#"
        'draw worm head
        Console.SetCursorPosition(worm(wormHeadPointer, 1), worm(wormHeadPointer, 2))

        Console.Write("#")

        'delete segment from tail
        If wormGrow = False Then
            Console.SetCursorPosition(worm(wormTailPointer, 1), worm(wormTailPointer, 2))

            Console.Write(" ")
            arena(worm(wormTailPointer, 1), worm(wormTailPointer, 2)) = " "
            wormTailPointer = wormTailPointer + 1
            If (wormTailPointer = 100) Then wormTailPointer = 0
        Else
            wormGrow = False
        End If

    End Sub



    Sub getInput()
        Dim key As System.ConsoleKeyInfo
        If Console.KeyAvailable Then key = Console.ReadKey(True)

        If ((key.KeyChar = "a") And (wormDirection <> "r")) Then wormDirection = "l"
        If ((key.KeyChar = "d") And (wormDirection <> "l")) Then wormDirection = "r"
        If ((key.KeyChar = "w") And (wormDirection <> "d")) Then wormDirection = "u"
        If ((key.KeyChar = "s") And (wormDirection <> "u")) Then wormDirection = "d"
    End Sub

    Sub spawnFood()
        Dim x, y As Integer
        Do
            x = Rnd() * 49
            y = Rnd() * 23
        Loop Until (arena(x, y) = " ")
        arena(x, y) = "+"
        Console.SetCursorPosition(x, y)
        Console.Write("+")
        foodTimer = 200

    End Sub

    Sub collisionDetectionAndScore()
        If arena(worm(wormHeadPointer, 1), worm(wormHeadPointer, 2)) = "+" Then
            wormGrow = True
            score = score + 10 + foodTimer
            Console.SetCursorPosition(55, 2)
            Console.Write("SCORE: {0}", score)
            gameSpeed = gameSpeed - 1
            spawnFood()
        End If
        If arena(worm(wormHeadPointer, 1), worm(wormHeadPointer, 2)) = "#" Then wormAlive = False

    End Sub

    Sub gameEnd()
        Dim key As Char
        Console.SetCursorPosition(17, 12)
        Console.Write("G A M E  O V E R")
        System.Threading.Thread.Sleep(3000)
        Console.SetCursorPosition(14, 14)
        Console.Write("Press a key to continue")
        key = Console.ReadKey.KeyChar

    End Sub

    Sub initialiseHighScores()
        Dim counter As Integer
        For counter = 0 To 9
            highScoreName(counter) = "Wormer"
            highScoreValue(counter) = 0
        Next

    End Sub

    Sub highScores()
        Dim counter1, counter2 As Integer
        Dim key As Char
        Dim isHighScore As Boolean
        Dim name As String

        'check if score is a high score
        isHighScore = False
        counter1 = 0

        'go through scores and check if score is a high score
        Do
            If (score > highScoreValue(counter1)) Then isHighScore = True
            If (isHighScore <> True) Then counter1 = counter1 + 1
        Loop Until (counter1 = 9) Or (isHighScore = True)
        'if it is a high score, ask for name and add to list
        If (isHighScore = True) Then
            Console.Clear()
            Console.SetCursorPosition(5, 5)
            Console.Write("N E W  H I G H  S C O R E")
            Console.SetCursorPosition(5, 7)
            Console.Write("Please enter your name: ")
            name = Console.ReadLine()
        End If

        ' move other scores down one place
        For counter2 = 9 To counter1 + 1 Step -1
            highScoreName(counter2) = highScoreName(counter2 - 1)
            highScoreValue(counter2) = highScoreValue(counter2 - 1)
        Next

        'add high score to list
#Disable Warning BC42104 ' Variable is used before it has been assigned a value
        highScoreName(counter1) = name
#Enable Warning BC42104 ' Variable is used before it has been assigned a value
        highScoreValue(counter1) = score

        'output high scores
        Console.Clear()
        Console.SetCursorPosition(5, 5)
        Console.Write("T O D A Y 'S  H I G H  S C O R E S")
        For counter1 = 0 To 9
            Console.SetCursorPosition(5, 7 + counter1)
            Console.Write(highScoreName(counter1))
            Console.SetCursorPosition(20, 7 + counter1)
            Console.Write(highScoreValue(counter1))
        Next
        Console.SetCursorPosition(5, 23)
        Console.Write("Press a key to play")
        key = Console.ReadKey.KeyChar

    End Sub

    Sub updateFoodTimer()
        If (foodTimer > 0) Then foodTimer = foodTimer - 1
        Console.SetCursorPosition(55, 4)
        Console.Write("Bonus: {0}", foodTimer)
    End Sub

    Sub writeToFile()
        FileOpen(1, "C:\Users\9876785\source\repos\snake", OpenMode.Output)

    End Sub


End Module
