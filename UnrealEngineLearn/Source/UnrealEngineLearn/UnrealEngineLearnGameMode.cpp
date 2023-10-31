// Copyright Epic Games, Inc. All Rights Reserved.

#include "UnrealEngineLearnGameMode.h"
#include "UnrealEngineLearnHUD.h"
#include "UnrealEngineLearnCharacter.h"
#include "UObject/ConstructorHelpers.h"

AUnrealEngineLearnGameMode::AUnrealEngineLearnGameMode()
	: Super()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnClassFinder(TEXT("/Game/FirstPersonCPP/Blueprints/FirstPersonCharacter"));
	DefaultPawnClass = PlayerPawnClassFinder.Class;

	// use our custom HUD class
	HUDClass = AUnrealEngineLearnHUD::StaticClass();
}
