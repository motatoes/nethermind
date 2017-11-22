﻿using System;
using System.Collections.Generic;
using System.IO;
using Ethereum.Blockchain.Test;

namespace Nevermind.Blockchain.Test.Runner
{
    public class BugHunter : BlockchainTestBase, ITestInRunner
    {
        public CategoryResult RunTests(string subset, int iterations = 1)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            List<string> failingTests = new List<string>();

            string directoryName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "FailingTests");
            IEnumerable<BlockchainTest> tests = LoadTests(subset);
            foreach (BlockchainTest test in tests)
            {
                Setup(null);

                try
                {
                    Console.Write($"{test.Name,-80} ");
                    RunTest(test);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("PASS");
                    Console.ForegroundColor = defaultColor;
                }
                catch (Exception)
                {
                    failingTests.Add(test.Name);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FAIL");
                    Console.ForegroundColor = defaultColor;
                    FileLogger logger = new FileLogger(Path.Combine(directoryName, string.Concat(subset, "_", test.Name, ".txt")));
                    try
                    {
                        if (!Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }

                        Setup(logger);
                        RunTest(test);
                    }
                    catch (Exception ex)
                    {
                        logger.Log(ex.ToString());
                        logger.Flush();
                    }

                    // should not happend
                    logger.Flush();
                }
            }

            return new CategoryResult(0, failingTests.ToArray());
        }
    }
}