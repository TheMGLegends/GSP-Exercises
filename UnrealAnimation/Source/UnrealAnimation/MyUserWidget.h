// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Blueprint/UserWidget.h"
#include "MyUserWidget.generated.h"

DECLARE_DYNAMIC_MULTICAST_DELEGATE(FMyBindableEvent);

/**
 * 
 */
UCLASS()
class UNREALANIMATION_API UMyUserWidget : public UUserWidget
{
	GENERATED_BODY()
	
public:
	// -- This is my custom event that will run when I click on the button -- //
	UFUNCTION(BlueprintCallable, Category = "Custom UI C++ Tutorials")
	void TestButtonClick();

	UPROPERTY(BlueprintAssignable)
	FMyBindableEvent OnCustomFire;

//public:
//	// -- I will use this to change the Text of the Textblock! -- //
//	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Custom UI C++ Tutorials")
//	FString MyCustomTextProperty;


};
