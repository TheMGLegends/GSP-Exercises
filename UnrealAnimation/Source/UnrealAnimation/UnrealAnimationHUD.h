// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once 

#include "CoreMinimal.h"
#include "GameFramework/HUD.h"
#include "UnrealAnimationHUD.generated.h"

UCLASS()
class AUnrealAnimationHUD : public AHUD
{
	GENERATED_BODY()

public:
	AUnrealAnimationHUD();

	/** Primary draw call for the HUD */
	virtual void DrawHUD() override;

private:
	/** Crosshair asset pointer */
	class UTexture2D* CrosshairTex;

};

