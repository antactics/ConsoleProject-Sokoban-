namespace _250319_콘솔프로젝트제작
{
    internal class Program
    {
        struct Position
        {
            public int x;
            public int y;
        }



        static void Main(string[] args)
        {

            /* 실습목표
             Console Clear 기능으로 이동 후 새로 그리는 개념
            2차원 배열의 요소에 따라 이동 가능/불가능 판단
            게임적인 메커니즘 추가하여 목표 달성
             */

            //게임루프 작성 INPUT-UPDATE-RENDER

            //gameOver라는 변수는 false로 초기화
            bool gameOver = false;
            bool deadEnd = false;
            bool hasWeapon = false;
            Position playerPos;
            char[,] map;

            Start(out playerPos, out map);
            while (gameOver == false)
            {
                //gameOver와 deadEnd 모두 false가 아닌 한 반복한다. Render, Input Update를
                Render(playerPos, map, hasWeapon);
                ConsoleKey key = Input();
                Update(key, ref playerPos, map, ref gameOver, ref deadEnd, ref hasWeapon);  //키와 플레이어 위치 원본을 넣어줘야 움직임 변경이 가능

                if (deadEnd == true)
                {
                    Console.Clear();
                    DeadEnd();
                    gameOver = true;  // gameOver는 아직 false이니 true로 강제로 바꿔줌
                    return;
                }
            }
            End();


        }
        //플레이어의 위치 값 x, y를 Position에 담음 

        //게임의 초기 설정
        static void Start(out Position playerPos, out char[,] map)
        {
            // 게임설정
            Console.CursorVisible = false;
            // 플레이어 위치설정
            playerPos.x = 7;
            playerPos.y = 7;

            // 맵 설정
            map = new char[9, 9]
            {
                {'▒', '▒', '▒', '▒', '▒', '▒', '▒', '▒', '▒'},
                {'▒', ' ', ' ', ' ', '□', ' ', '▒', '□', '▒'},
                {'▒', ' ', ' ', ' ', ' ', ' ', '▒', ' ', '▒'},
                {'▒', '□', '■', ' ', '■', ' ', '▒', '■', '▒'},
                {'▒', ' ', ' ', ' ', '▒', '★', ' ', '◆', '▒'},
                {'▒', ' ', ' ', ' ', ' ', '▒', '▒', ' ', '▒'},  //◆ =적
                {'▒', ' ', ' ', '□', '■', ' ', '▒', ' ', '▒'},  //★ =무기
                {'▒', ' ', ' ', ' ', '▒', ' ', ' ', ' ', '▒'},
                {'▒', '▒', '▒', '▒', '▒', '▒', '▒', '▒', '▒'}
            };

            ShowTitle();
        }



        static void ShowTitle()
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("|                                                    |");
            Console.WriteLine("|                 [Push and Escape]                  |");
            Console.WriteLine("|                                                    |");
            Console.WriteLine("| 당신은 미로에 갇혔습니다.                          |");
            Console.WriteLine("| 박스(■)를 골인지점(□)에 모두 밀어 넣으세요.        |");
            Console.WriteLine("| 적(◆)과 부딪히게 되면 게임 오버입니다.             |");
            Console.WriteLine("| 무기(★)를 획득하면 적을 무찌를 수 있습니다.        |");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("아무키나 눌러 게임을 시작하세요.");



            Console.ReadKey(true);
            Console.Clear();
        }

        static void Render(Position playerPos, char[,] map, bool hasWeapon)
        {
            Console.SetCursorPosition(0, 0);
            PrintMap(map);
            PrintPlayer(playerPos, map, hasWeapon);
        }

        static void PrintMap(char[,] map)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    Console.Write(map[y, x]);
                }
                Console.WriteLine();
            }
        }

        static void PrintPlayer(Position playerPos, char[,] map, bool hasWeapon)
        {
            Console.SetCursorPosition(playerPos.x, playerPos.y);
            Console.ForegroundColor = ConsoleColor.Green;
            if (hasWeapon)
            {
                Console.Write('★');
            }
            else
            {
                Console.Write('▼');
            }


            Console.ResetColor();
        }



        static ConsoleKey Input()
        {
            return Console.ReadKey(true).Key;
        }

        static void Update(ConsoleKey key, ref Position playerPos, char[,] map, ref bool gameOver, ref bool deadEnd, ref bool hasWeapon) //입력한 내용으로 플레이어 위치를 이동
        {
            Move(key, ref playerPos, map, ref hasWeapon, ref deadEnd);

            bool isClear = IsClear(map);
            if (isClear)
            {
                gameOver = true;
            }

        }

        static void Move(ConsoleKey key, ref Position playerPos, char[,] map, ref bool hasWeapon, ref bool deadEnd)
        {
            switch (key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    //움직이는 방향에 박스가 있으면
                    if (map[playerPos.y, playerPos.x - 1] == '■')
                    {
                        //1.박스의 목적지에 벽이나 박스인 경우
                        if (map[playerPos.y, playerPos.x - 2] == '▒' ||
                              map[playerPos.y, playerPos.x - 2] == '■')
                        {
                            //밀리면 안된다

                        }
                        //2.목적지가 빈칸인 경우
                        else if (map[playerPos.y, playerPos.x - 2] == ' ')
                        {
                            //원래자리는 빈공간으로 만들고 새로운자리에 박스를 만든다 
                            map[playerPos.y, playerPos.x - 1] = ' ';
                            map[playerPos.y, playerPos.x - 2] = '■';
                            playerPos.x--;
                        }
                        //3.목적지가 Goal인 경우
                        else if (map[playerPos.y, playerPos.x - 2] == '□')
                        {
                            //박스를 밀어 골에 넣고 이동한다.
                            map[playerPos.y, playerPos.x - 1] = ' ';
                            map[playerPos.y, playerPos.x - 2] = '▣';
                            playerPos.x--;
                        }
                    }
                    //움직이려는 방향에 골안 박스가 있을때
                    else if (map[playerPos.y, playerPos.x - 1] == '▣')
                    {
                        //1. 목적지가 빈칸이면 골을 만들고 밀어
                        if (map[playerPos.y, playerPos.x - 2] == ' ')
                        {
                            map[playerPos.y, playerPos.x - 1] = '□';
                            map[playerPos.y, playerPos.x - 2] = '■';
                            playerPos.x--;
                        }
                        //2. 목적지가 골인 경우
                        else if (map[playerPos.y, playerPos.x - 2] == '□')
                        {
                            map[playerPos.y, playerPos.x - 1] = '□';
                            map[playerPos.y, playerPos.x - 2] = '▣';
                            playerPos.x--;
                        }
                    }
                    //빈칸이면 이동한다
                    else if (map[playerPos.y, playerPos.x - 1] == ' ' ||
                        map[playerPos.y, playerPos.x - 1] == '□')
                    {
                        playerPos.x--;
                    }

                    // 적을 무방비로 만나면 죽는다. 무기를 가졌다면 물리친다.
                    else if (map[playerPos.y, playerPos.x - 1] == '◆')
                    {
                        if (hasWeapon)
                        {
                            map[playerPos.y, playerPos.x - 1] = ' ';
                            playerPos.x--;
                        }
                        else
                        {
                            deadEnd = true;
                        }
                    }

                    //무기를 얻으면 ★로 변한다
                    else if (map[playerPos.y, playerPos.x - 1] == '★')
                    {
                        map[playerPos.y, playerPos.x - 1] = ' ';
                        playerPos.x--;
                        hasWeapon = true;
                    }



                    break;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    //움직이는 방향에 박스가 있으면
                    if (map[playerPos.y, playerPos.x + 1] == '■')
                    {
                        //1.박스의 목적지에 벽이나 박스인 경우
                        if (map[playerPos.y, playerPos.x + 2] == '▒' ||
                              map[playerPos.y, playerPos.x + 2] == '■')
                        {
                            //밀리면 안된다

                        }
                        //2.목적지가 빈칸인 경우
                        else if (map[playerPos.y, playerPos.x + 2] == ' ')
                        {
                            //원래자리는 빈공간으로 만들고 새로운자리에 박스를 만든다 
                            map[playerPos.y, playerPos.x + 1] = ' ';
                            map[playerPos.y, playerPos.x + 2] = '■';
                            playerPos.x++;
                        }
                        //3.목적지가 Goal인 경우
                        else if (map[playerPos.y, playerPos.x + 2] == '□')
                        {
                            //박스를 밀어 골에 넣고 이동한다.
                            map[playerPos.y, playerPos.x + 1] = ' ';
                            map[playerPos.y, playerPos.x + 2] = '▣';
                            playerPos.x++;
                        }
                    }
                    //움직이려는 방향에 골안 박스가 있을때
                    else if (map[playerPos.y, playerPos.x + 1] == '▣')
                    {
                        //1. 목적지가 빈칸이면 골을 만들고 밀어
                        if (map[playerPos.y, playerPos.x + 2] == ' ')
                        {
                            map[playerPos.y, playerPos.x + 1] = '□';
                            map[playerPos.y, playerPos.x + 2] = '■';
                            playerPos.x++;
                        }
                        //2. 목적지가 골인 경우
                        else if (map[playerPos.y, playerPos.x + 2] == '□')
                        {
                            map[playerPos.y, playerPos.x + 1] = '□';
                            map[playerPos.y, playerPos.x + 2] = '▣';
                            playerPos.x++;
                        }
                    }
                    else if (map[playerPos.y, playerPos.x + 1] == ' ' ||
                        map[playerPos.y, playerPos.x + 1] == '□')
                    {
                        playerPos.x++;
                    }

                    // 적을 무방비로 만나면 죽는다. 무기를 가졌다면 물리친다.
                    else if (map[playerPos.y, playerPos.x + 1] == '◆')
                    {
                        if (hasWeapon)
                        {
                            map[playerPos.y, playerPos.x + 1] = ' ';
                            playerPos.x++;
                        }
                        else
                        {
                            deadEnd = true;
                        }
                    }

                    //무기를 얻으면 ★로 변한다
                    else if (map[playerPos.y, playerPos.x + 1] == '★')
                    {
                        map[playerPos.y, playerPos.x + 1] = ' ';
                        playerPos.x++;
                        hasWeapon = true;
                    }


                    break;

                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    //움직이는 방향에 박스가 있으면
                    if (map[playerPos.y - 1, playerPos.x] == '■')
                    {
                        //1.박스의 목적지에 벽이나 박스인 경우
                        if (map[playerPos.y - 2, playerPos.x] == '▒' ||
                              map[playerPos.y - 2, playerPos.x] == '■')
                        {
                            //밀리면 안된다

                        }
                        //2.목적지가 빈칸인 경우
                        else if (map[playerPos.y - 2, playerPos.x] == ' ')
                        {
                            //원래자리는 빈공간으로 만들고 새로운자리에 박스를 만든다 
                            map[playerPos.y - 1, playerPos.x] = ' ';
                            map[playerPos.y - 2, playerPos.x] = '■';
                            playerPos.y--;
                        }
                        //3.목적지가 Goal인 경우
                        else if (map[playerPos.y - 2, playerPos.x] == '□')
                        {
                            //박스를 밀어 골에 넣고 이동한다.
                            map[playerPos.y - 1, playerPos.x] = ' ';
                            map[playerPos.y - 2, playerPos.x] = '▣';
                            playerPos.y--;
                        }
                    }
                    //움직이려는 방향에 골안 박스가 있을때
                    else if (map[playerPos.y - 1, playerPos.x] == '▣')
                    {
                        //1. 목적지가 빈칸이면 골을 만들고 밀어
                        if (map[playerPos.y - 2, playerPos.x] == ' ')
                        {
                            map[playerPos.y - 1, playerPos.x] = '□';
                            map[playerPos.y - 2, playerPos.x] = '■';
                            playerPos.y--;
                        }
                        //2. 목적지가 골인 경우
                        else if (map[playerPos.y - 2, playerPos.x] == '□')
                        {
                            map[playerPos.y - 1, playerPos.x] = '□';
                            map[playerPos.y - 2, playerPos.x] = '▣';
                            playerPos.y--;
                        }
                    }
                    else if (map[playerPos.y - 1, playerPos.x] == ' ' ||
                        map[playerPos.y - 1, playerPos.x] == '□')
                    {
                        playerPos.y--;
                    }

                    // 적을 무방비로 만나면 죽는다. 무기를 가졌다면 물리친다.
                    else if (map[playerPos.y - 1, playerPos.x] == '◆')
                    {
                        if (hasWeapon)
                        {
                            map[playerPos.y - 1, playerPos.x] = ' ';
                            playerPos.y--;
                        }
                        else
                        {
                            deadEnd = true;
                        }
                    }

                    //무기를 얻으면 ★로 변한다
                    else if (map[playerPos.y - 1, playerPos.x] == '★')
                    {
                        map[playerPos.y - 1, playerPos.x] = ' ';
                        playerPos.y--;
                        hasWeapon = true;
                    }
                    break;


                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    //움직이는 방향에 박스가 있으면
                    if (map[playerPos.y + 1, playerPos.x] == '■')
                    {
                        //1.박스의 목적지에 벽이나 박스인 경우
                        if (map[playerPos.y + 2, playerPos.x] == '▒' ||
                              map[playerPos.y + 2, playerPos.x] == '■')
                        {
                            //밀리면 안된다

                        }
                        //2.목적지가 빈칸인 경우
                        else if (map[playerPos.y + 2, playerPos.x] == ' ')
                        {
                            //원래자리는 빈공간으로 만들고 새로운자리에 박스를 만든다 
                            map[playerPos.y + 1, playerPos.x] = ' ';
                            map[playerPos.y + 2, playerPos.x] = '■';
                            playerPos.y++;
                        }
                        //3.목적지가 Goal인 경우
                        else if (map[playerPos.y + 2, playerPos.x] == '□')
                        {
                            //박스를 밀어 골에 넣고 이동한다.
                            map[playerPos.y + 1, playerPos.x] = ' ';
                            map[playerPos.y + 2, playerPos.x] = '▣';
                            playerPos.y++;
                        }
                    }
                    //움직이려는 방향에 골안 박스가 있을때
                    else if (map[playerPos.y + 1, playerPos.x] == '▣')
                    {
                        //1. 목적지가 빈칸이면 골을 만들고 밀어
                        if (map[playerPos.y + 2, playerPos.x] == ' ')
                        {
                            map[playerPos.y + 1, playerPos.x] = '□';
                            map[playerPos.y + 2, playerPos.x] = '■';
                            playerPos.y++;
                        }
                        //2. 목적지가 골인 경우
                        else if (map[playerPos.y + 2, playerPos.x] == '□')
                        {
                            map[playerPos.y + 1, playerPos.x] = '□';
                            map[playerPos.y + 2, playerPos.x] = '▣';
                            playerPos.y++;
                        }
                    }
                    else if (map[playerPos.y + 1, playerPos.x] == ' ' ||
                        map[playerPos.y + 1, playerPos.x] == '□')
                    {
                        playerPos.y++;
                    }

                    // 적을 무방비로 만나면 죽는다. 무기를 가졌다면 물리친다.
                    else if (map[playerPos.y, playerPos.x - 1] == '◆')
                    {
                        if (hasWeapon)
                        {
                            map[playerPos.y + 1, playerPos.x] = ' ';
                            playerPos.y++;
                        }
                        else
                        {
                            deadEnd = true;
                        }
                    }

                    //무기를 얻으면 ★로 변한다
                    else if (map[playerPos.y + 1, playerPos.x] == '★')
                    {

                        map[playerPos.y + 1, playerPos.x] = ' ';
                        playerPos.y++;
                        hasWeapon = true;

                    }

                    break;
            }

        }


        static bool IsClear(char[,] map)
        {

            int goalCount = 0;
            //클리어 : 빈 골이 없을때
            //포이치 맵을 처음부터 끝까지 하나 하나 다 확인함
            foreach (char tile in map)
            {
                if (tile == '□')
                {
                    goalCount++;
                    break;
                    //return false;
                }
            }

            if (goalCount == 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        //게임의 끝 작업
        static void End()
        {
            Console.Clear();
            Console.WriteLine("   Congratulations!   ");
            Console.WriteLine("게임을 클리어했습니다.");
        }

        static void DeadEnd()
        {
            Console.Clear();
            Console.WriteLine("             You Died              ");
            Console.WriteLine("무방비 상태로 적을 만나 죽었습니다.");
        }

    }
}