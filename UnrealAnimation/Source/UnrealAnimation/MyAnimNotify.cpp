// Fill out your copyright notice in the Description page of Project Settings.


#include "MyAnimNotify.h"

void UMyAnimNotify::Notify(USkeletalMeshComponent* MeshComp, UAnimSequenceBase* Animation)
{
	GLog->Log(TEXT("Animation Hit Notifying!"));

	UE_LOG(LogTemp, Log, TEXT("My Actor name is %s"), *MeshComp->GetOwner()->GetActorLabel());

	UE_LOG(LogTemp, Log, TEXT("Cool Variable: %d"), CoolProperty);
}
