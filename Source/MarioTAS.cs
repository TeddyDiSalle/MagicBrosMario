using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MagicBrosMario.Source.GameStates;
using MagicBrosMario.Source.Level;

namespace MagicBrosMario.Source;

public class MarioTAS
{
    public static MarioTAS Instance { get; } = new MarioTAS();

    private struct TasStep
    {
        public double Duration;
        public Action<GameTime> Action;

        public TasStep(double duration, Action<GameTime> action)
        {
            Duration = duration;
            Action = action;
        }
    }

    private readonly List<TasStep> steps = new();
    private int currentStep = 0;
    private double stepTimer = 0;
    private bool running = false;

    private MarioTAS() { }

    public bool IsRunning => running;

    public void BeatLevel1_1()
    {
        LoadScript(new List<TasStep>
        {
            Step(0, LoadLevel1),
            Step(1.0, Wait),

            Step(4.0, RunRight),
            Step(0.45, JumpRight),
            Step(3.0, RunRight),
            Step(0.40, JumpRight),
            Step(5.0, RunRight),
            Step(0.50, JumpRight),
            Step(6.0, RunRight),

            Step(0, FinishLevel1)
        });
    }

    public void BeatLevel1_2()
    {
        LoadScript(new List<TasStep>
        {
            Step(0, LoadLevel2),
            Step(1.0, Wait),

            Step(3.5, RunRight),
            Step(0.45, JumpRight),
            Step(2.5, RunRight),
            Step(0.50, JumpRight),
            Step(5.0, RunRight),
            Step(0.40, JumpRight),
            Step(6.0, RunRight),

            Step(0, FinishLevel2)
        });
    }

    public void ExecuteTAS()
    {
        LoadScript(new List<TasStep>
        {
            Step(0, LoadLevel1),
            Step(1.0, Wait),

            Step(4.0, RunRight),
            Step(0.45, JumpRight),
            Step(3.0, RunRight),
            Step(0.40, JumpRight),
            Step(5.0, RunRight),
            Step(0.50, JumpRight),
            Step(6.0, RunRight),

            Step(0, FinishLevel1),

            Step(0, LoadLevel2),
            Step(1.0, Wait),

            Step(3.5, RunRight),
            Step(0.45, JumpRight),
            Step(2.5, RunRight),
            Step(0.50, JumpRight),
            Step(5.0, RunRight),
            Step(0.40, JumpRight),
            Step(6.0, RunRight),

            Step(0, FinishLevel2)
        });
    }

    public void Stop()
    {
        running = false;
        steps.Clear();
        currentStep = 0;
        stepTimer = 0;

        if (MagicBrosMario.INSTANCE?.Mario != null)
        {
            MagicBrosMario.INSTANCE.Mario.Idle();
        }
    }

    public void Update(GameTime gt)
    {
        if (!running || steps.Count == 0)
            return;

        while (currentStep < steps.Count && steps[currentStep].Duration <= 0)
        {
            steps[currentStep].Action(gt);
            currentStep++;
            stepTimer = 0;
        }

        if (currentStep >= steps.Count)
        {
            Stop();
            return;
        }

        TasStep step = steps[currentStep];

        step.Action(gt);
        stepTimer += gt.ElapsedGameTime.TotalSeconds;

        if (stepTimer >= step.Duration)
        {
            currentStep++;
            stepTimer = 0;
        }
    }

    private void LoadScript(List<TasStep> script)
    {
        steps.Clear();
        steps.AddRange(script);
        currentStep = 0;
        stepTimer = 0;
        running = true;
    }

    private static TasStep Step(double duration, Action<GameTime> action)
    {
        return new TasStep(duration, action);
    }

    private static void LoadLevel1(GameTime gt)
    {
        MagicBrosMario.INSTANCE.finishedLevel1 = false;
        MagicBrosMario.INSTANCE.CurrentState = new TransitionState(new Level1());
    }

    private static void LoadLevel2(GameTime gt)
    {
        MagicBrosMario.INSTANCE.finishedLevel1 = true;
        MagicBrosMario.INSTANCE.CurrentState = new TransitionState(new Level2());
    }

    private static void FinishLevel1(GameTime gt)
    {
        MagicBrosMario.INSTANCE.finishedLevel1 = true;
        Console.WriteLine("TAS finished Level 1!");
    }

    private static void FinishLevel2(GameTime gt)
    {
        MagicBrosMario.INSTANCE.finishedLevel2 = true;
        Console.WriteLine("TAS finished Level 2!");
    }

    private static void Wait(GameTime gt)
    {
        MagicBrosMario.INSTANCE.Mario.Idle();
    }

    private static void RunRight(GameTime gt)
    {
        MagicBrosMario.INSTANCE.Mario.Sprint();
        MagicBrosMario.INSTANCE.Mario.Right(gt);
    }

    private static void JumpRight(GameTime gt)
    {
        MagicBrosMario.INSTANCE.Mario.Sprint();
        MagicBrosMario.INSTANCE.Mario.Jump(gt);
        MagicBrosMario.INSTANCE.Mario.Right(gt);
    }

    private static void WalkLeft(GameTime gt)
    {
        MagicBrosMario.INSTANCE.Mario.Left(gt);
    }

    private static void Crouch(GameTime gt)
    {
        MagicBrosMario.INSTANCE.Mario.Crouch(gt);
    }

    private static void Attack(GameTime gt)
    {
        MagicBrosMario.INSTANCE.Mario.Attack();
    }
}